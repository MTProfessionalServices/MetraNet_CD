﻿Ext.namespace('Ext.ux');
Ext.ux.JPathJsonReader = Ext.extend(Ext.data.JsonReader, {
    getJPathVal: function(obj, f){
        var jp = new JPath(obj);
        return jp.query(f.mapping)
    },
    readRecords: function(o){
    
        this.jsonData = o;
        var s = this.meta, Record = this.recordType, f = Record.prototype.fields, fi = f.items, fl = f.length;
        
        
        if (!this.ef) {
            if (s.totalProperty) {
                this.getTotal = this.getJsonAccessor(s.totalProperty);
            }
            if (s.successProperty) {
                this.getSuccess = this.getJsonAccessor(s.successProperty);
            }
            this.getRoot = s.root ? this.getJsonAccessor(s.root) : function(p){
                return p;
            };
            if (s.id) {
                var g = this.getJsonAccessor(s.id);
                this.getId = function(rec){
                    var r = g(rec);
                    return (r === undefined || r === "") ? null : r;
                };
            }
            else {
                this.getId = function(){
                    return null;
                };
            }
            this.ef = [];
            for (var i = 0; i < fl; i++) {
                f = fi[i];
                var map = (f.mapping !== undefined && f.mapping !== null) ? f.mapping : f.name;
                this.ef[i] = this.getJsonAccessor(map);
            }
        }
        
        var root = this.getRoot(o), c = root.length, totalRecords = c, success = true;
        if (s.totalProperty) {
            var v = parseInt(this.getTotal(o), 10);
            if (!isNaN(v)) {
                totalRecords = v;
            }
        }
        if (s.successProperty) {
            var v = this.getSuccess(o);
            if (v === false || v === 'false') {
                success = false;
            }
        }
        var records = [];
        for (var i = 0; i < c; i++) {
            var n = root[i];
            var values = {};
            var id = this.getId(n);
            for (var j = 0; j < fl; j++) {
                f = fi[j];
                var v = "";
                (f.useJPath !== undefined && f.useJPath !== null && f.useJPath)
                ? v = this.getJPathVal(n,f)
                : v = this.ef[j](n)
                values[f.name] = f.convert((v !== undefined) ? v : f.defaultValue, n);
            }
            var record = new Record(values, id);
            record.json = n;
            records[i] = record;
        }
        return {
            success: success,
            records: records,
            totalRecords: totalRecords
        };
    }
})



/*
   JPath 1.0.3 - json equivalent to xpath
   Copyright (C) 2007  Bryan English <bryan at bluelinecity dot com>

   This program is free software; you can redistribute it and/or
   modify it under the terms of the GNU General Public License
   as published by the Free Software Foundation; either version 2
   of the License, or (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

   Usage:      
      var jpath = new JPath( myjsonobj );

      var somevalue = jpath.$('book/title').json;  //results in title
         //or
      var somevalue = jpath.query('book/title');   //results in title

   Supported XPath-like Syntax:
      /tagname
      //tagname
      tagname
      * wildcard
      [] predicates
      operators ( >=, ==, <= )
      array selection
      .. 	         
      *
      and, or
      nodename[0]
      nodename[last()]
      nodename[position()]
      nodename[last()-1]
      nodename[somenode > 3]/node
      nodename[count() > 3]/node

   Tested With:
      Firefox 2-3, IE 6-7
   
   Update Log:
      1.0.1 - Bugfix for zero-based element selection
      1.0.2 - Bugfix for IE not handling eval() and returning a function
      1.0.3 - Bugfix added support for underscore and dash in query() function
                  Bugfix improper use of Array.concat which was flattening arrays
                  Added support for single equal sign in query() function
                  Added support for count() xpath function
                  Added support for and, or boolean expression in predicate blocks
                  Added support for global selector $$ and //
                  Added support for wildcard (*) selector support 
*/

function JPath( json, parent )
{ 
    this.json = json; 
    this._parent = parent; 
}

JPath.prototype = {

   /*
      Property: json
      Copy of current json segment to operate on
   */
   json: null,
   
   /*
      Property: parent
      Parent json object, null if root.
   */
   parent: null,

   /*
      Method: $
      Performs a find query on the current jpath object.

      Parameters:
        str - mixed, find query to perform. Can consist of a nodename or nodename path or function object or integer.

      Return:
        jpath - Returns the resulting jpath object after performing find query.

   */
   '$': function ( str )
   {
      var result = null;
      var working = this;
      
      if ( this.json && str !== null )
      {
         switch ( typeof(str) )
         {
            case 'function':
               result = this.f(str).json;
            break;

            case 'number':
               result = this.json[str] || null;
            break;

            case 'string':
               var names = str.split('/');     

               //foreach slash delimited node name//
               for ( var i=0; i<names.length ; i++ )
               {
                  var name = new RegExp('^' + names[i].replace(/\*/g,'.*') + '$');                  
                  var isArray = (working.json instanceof Array);
                  var a = new Array();
                  
                  //foreach working node property//
                  for ( var p in working.json )
                  {
                     if ( typeof( working.json[p] ) != 'function' )
                     {
                        if ( isArray && (arguments.callee.caller != this.$$) )
                        {
                           a = a.concat( this.findAllByRegExp( name, working.json[p] ) );
                        }
                        else if ( name.test(p) )
                        {                        
                           a.push( working.json[p] );
                        }
                     }                  
                  }

                  working = new JPath( ( a.length==0 ? null : ( ( a.length == 1) ? a[0] : a ) ), working );
               }

               return working;
            break;
         }   
      }
      
      return new JPath( result, this );
   },

   /*
      Method: $$
      Performs a global, recursive find query on the current jpath object.

      Parameters:
        str - mixed, find query to perform. Can consist of a nodename or nodename path or function object or integer.

      Return:
        jpath - Returns the resulting jpath object after performing find query.

   */   
   '$$': function( str )
   {   
      var r = this.$(str,true).json;
      var arr = new Array();
      
      if ( r instanceof Array ) 
         arr = arr.concat(r); 
      else if ( r !== null )
         arr.push(r);
         
      for ( var p in this.json )
      {
         if ( typeof( this.json[p] ) == 'object' )
         {
            arr = arr.concat( new JPath( this.json[p], this ).$$(str).json );
         }
      }
      
      return new JPath( arr, this );
   },
   
   /*
      Method: findAllByRegExp
      Looks through a list of an object properties using a regular expression

      Parameters:
         re - regular expression, to use to search with
         obj - object, the object to search through

      Returns:
         array - resulting properties
   */
   findAllByRegExp: function( re, obj )
   {
      var a = new Array();
   
      for ( var p in obj )
      {
         if ( obj instanceof Array )
         {
            a = a.concat( this.findAllByRegExp( re, obj[p] ) );
         }
         else if ( typeof( obj[p] ) != 'function' && re.test(p) )
         {
            a.push( obj[p] );
         }
      }

      return a;
   },

   /*
      Method: query (beta)
      Performs a find query on the current jpath object using a single string similar to xpath. This method
      is currently expirimental.

      Parameters:
        str - string, full xpath-like query to perform on the current object.

      Return:
        mixed - Returns the resulting json value after performing find query.

   */
   query: function( str )
   {
      var re = {
         " and ":" && ",
         " or ":" || ",
         "([\\#\\*\\@a-z\\_][\\*a-z0-9_\\-]*)(?=(?:\\s|$|\\[|\\]|\\/))" : "\$('$1').",
         "\\[([0-9])+\\]" : "\$($1).",
         "\\.\\." : "parent().",
         "\/\/" : "$",
         "(^|\\[|\\s)\\/" : "$1root().",
         "\\/" : '',
         "([^\\=\\>\\<\\!])\\=([^\\=])" : '$1==$2',
         "\\[" : "$(function(j){ with(j){return(",
         "\\]" : ");}}).",
         "\\(\\.":'(',
         "(\\.|\\])(?!\\$|\\p)":"$1json",
         "count\\(([^\\)]+)\\)":"count('$1')"
      };

      //save quoted strings//
      var quotes = /(\'|\")([^\1]*)\1/;
      var saves = new Array();
      while ( quotes.test(str) )
      {
         saves.push( str.match(quotes)[2] ); 
         str = str.replace(quotes,'%'+ (saves.length-1) +'%');
      }

      for ( var e in re )
      {
         str = str.replace( new RegExp(e,'ig'), re[e] );
      }
      //alert('this.' + str.replace(/\%(\d+)\%/g,'saves[$1]') + ";");

      return eval('this.' + str.replace(/\%(\d+)\%/g,'saves[$1]') + ";");
   },

   /*
      Method: f
      Performs the equivilant to an xpath predicate eval on the current nodeset.

      Parameters:
        f - function, an iterator function that is executed for every json node and is expected to return a boolean
        value which determines if that particular node is selected. Alternativly you can submit a string which will be
        inserted into a prepared function.

      Return:
        jpath - Returns the resulting jpath object after performing find query.

   */
   f: function ( iterator )
   {
      var a = new Array();

      if ( typeof(iterator) == 'string' )
      {
         eval('iterator = function(j){with(j){return('+ iterator +');}}');
      }

      for ( var p in this.json )
      {
         var j = new JPath(this.json[p], this);
         j.index = p;
         if ( iterator( j ) )
         {
            a.push( this.json[p] );
         }
      }

      return new JPath( a, this );
   },

   /*
      Method: parent
      Returns the parent jpath object or itself if its the root node

      Return:
        jpath - Returns the parent jpath object or itself if its the root node

   */
   parent: function()
   {
      return ( (this._parent) ? this._parent : this );
   },

   /*
      Method: position
      Returns the index position of the current node. Only valid within a function or predicate

      Return:
        int - array index position of this json object.
   */
   position: function()
   {
      return this.index;
   },

   /*
      Method: last
      Returns true if this is the last node in the nodeset. Only valid within a function or predicate

      Return:
        booean - Returns true if this is the last node in the nodeset
   */
   last: function()
   {
      return (this.index == (this._parent.json.length-1));
   },

   /*
      Method: count
      Returns the count of child nodes in the current node

      Parameters:
         string - optional node name to count, defaults to all
      
      Return:
        booean - Returns number of child nodes found
   */
   count: function(n)
   {
      var found = this.$( n || '*').json;         
      return ( found ? ( found instanceof Array ? found.length : 1 ) : 0 );
   },

   /*
      Method: root
      Returns the root jpath object.

      Return:
        jpath - The top level, root jpath object.
   */
   root: function ()
   {
      return ( this._parent ? this._parent.root() : this );
   }

};
