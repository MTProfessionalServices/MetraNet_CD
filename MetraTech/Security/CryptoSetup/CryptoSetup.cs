using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Xml;

using MetraTech.Security.Crypto;
using MetraTech.Security.DPAPI;
using MetraTech.Interop.RCD;
using CryptoConfig = MetraTech.Security.Crypto.CryptoConfig;

namespace MetraTech.Security.CryptoSetup
{
  class CryptoSetup
  {
    static void Main(string[] args)
    {
      //System.Threading.Thread.Sleep(10000);
      Command command = null;

      try
      {
        if (args.Length == 0)
        {
          PrintUsage();
          return;
        }

        switch (args[0].ToLower())
        {
          case "-help":
          case "/?":
            {
              PrintUsage();
              return;
            }
          case "-createkeys":
            {
              // Create RMP\config\security\sessionkeys.xml. 
              // Backup the original sessionkeys.xml to sessionkeys.xml.[Date].old
              command = new CreateKeysCommand(args);
              break;
            }
          case "-encryptconfig":
            {
              // Encrypt all passwords in config files. 
              // No keys will be generated hence, no change to RMP\config\security\sessionkeys.xml. 
              // This assumes that a valid RMP \config\security\sessionkeys.xml exists.
              command = new EncryptConfigCommand(args);
              break;
            }
          case "-pwd":
            {
              // Create one session key for each key class based on the specified password. 
              // Output is generated in RMP\config\security\sessionkeys.xml. 
              // The existing keys, if any, will be deleted.
              command = new PasswordCommand(args);
              break;
            }
          case "-keyclass":
            {
              // Create a session key for a specific key class based on a password. 
              // The existing keys for the specified keyclass will be deleted. 
              command = new KeyClassCommand(args);
              break;
            }
          case "-addkey":
            {
              // Add a session key for a specific key class based on a password and an identifier. 
              // The id must be a guid. Optionally, make the new key current. 
              // The existing keys for the specified key class will not be deleted. 
              command = new AddKeyCommand(args);
              break;
            }
          case "-current":
            {
              // Set a specific session key to be the current key 
              command = new CurrentKeyCommand(args);
              break;
            }
          case "-rsatest":
            {
              command = new RSATestCommand(args);
              break;
            }
          case "-dpde":
          case "-dpapidecrypt":
            {
              command = new DPAPIDecryptCommand(args);
              break;
            }
          case "-dpen":
          case "-dpapiencrypt":
            {
              command = new DPAPIEncryptCommand(args);
              break;
            }
          case "-msde":
          case "-msdecrypt":
            {
              bool needSfInitialize = true;
              command = new MSDecryptCommand(args, needSfInitialize);
              break;
            }
          case "-msen":
          case "-msencrypt":
            {
              command = new MSEncryptCommand(args);
              break;
            }
          case "-rsade":
          case "-rsadecrypt":
            {
              command = new RSADecryptCommand(args);
              break;
            }
          case "-rsaen":
          case "-rsaencrypt":
            {
              command = new RSAEncryptCommand(args);
              break;
            }
          case "-pwdhash":
            {
              command = new PasswordHash(args);
              break;
            }
          case "-test":
            {
              command = new TestCommand(args);
              break;
            }
          default:
            {
              Console.WriteLine(String.Format("The specified argument '{0}' is not recognized", args[0]));
              PrintUsage();
              return;
            }
        }

        command.Parse();
        if (command.HasErrors)
        {
          Console.WriteLine("\n");
          Console.WriteLine("Error parsing arguments.");
          Console.WriteLine("\n");
          Console.WriteLine(command.GetError());
          return;
        }

        command.Execute();

        // Write output
        if (!String.IsNullOrEmpty(command.Output))
        {
          Console.WriteLine("\n");
          Console.WriteLine(command.Output);
        }
      }
      catch (IndexOutOfRangeException e)
      {
        Console.WriteLine("\n");
        logger.LogException("CryptoSetup failed", e);
        PrintUsage();
      }
      catch (Exception)
      {
        // Print the output of the command execution, if any
        if (command != null)
        {
          if (command.HasErrors)
          {
            Console.WriteLine("\n");
            Console.WriteLine(command.GetError());
          }

          if (!String.IsNullOrEmpty(command.Output))
          {
            Console.WriteLine("\n");
            Console.WriteLine(command.Output);
          }
        }
        else
        {
          PrintUsage();
        }
      }
    }

    /// <summary>
    ///   Print usage
    /// </summary>
    /// <returns></returns>
    private static void PrintUsage()
    {
      string keyfile = MSSessionKeyConfig.GetInstance().KeyFile;

      Console.WriteLine("\nUsage:");
      Console.WriteLine("--------\n");

      Console.WriteLine("(1) CryptoSetup -createkeys");
      Console.WriteLine("    Create one session key for each key class based on a hard coded password ");
      Console.WriteLine("    Output is generated in RMP\\config\\security\\sessionkeys.xml. If there is");
      Console.WriteLine("    If there is an existing sessionkeys.xml file, it is copied to sessionkeys.xml.[Date].old");

      Console.WriteLine("\n");

      Console.WriteLine("(2) CryptoSetup -encryptconfig");
      Console.WriteLine("    Encrypt all passwords in config files. No keys will be generated.");
      Console.WriteLine("    This assumes that a valid RMP \\config\\security\\sessionkeys.xml exists");

      Console.WriteLine("\n");


      Console.WriteLine("(3) CryptoSetup -pwd pa$$w0rd");
      Console.WriteLine("    Create one session key for each key class based on the specified password.");
      Console.WriteLine("    Output is generated in RMP\\config\\security\\sessionkeys.xml.");
      Console.WriteLine("    Existing keys, if any, will be deleted");

      Console.WriteLine("\n");

      Console.WriteLine("(4) CryptoSetup –keyclass DatabasePassword –pwd pa$$w0rd");
      Console.WriteLine("    Create a session key for a specific key class based on a password.");
      Console.WriteLine("    The existing keys for the specified keyclass will be deleted.");

      Console.WriteLine("\n");

      Console.WriteLine("(5) CryptoSetup –addkey –keyclass DatabasePassword –pwd pa$$w0rd –id cbb23d8d-8189-4039-a575-a7388c369d99 [-makecurrent]");
      Console.WriteLine("    Add a session key for a specific key class based on a password and an id.");
      Console.WriteLine("    The id must be a Guid. Optionally, make the new key current. ");
      Console.WriteLine("    The existing keys for the specified key class will not be deleted. ");
      Console.WriteLine("    Note that new keys will need to be added to each machine that uses encryption.");

      Console.WriteLine("\n");

      Console.WriteLine("(6) CryptoSetup –current –id cbb23d8d-8189-4039-a575-a7388c369d99");
      Console.WriteLine("    Set a specific session key in a key class to be the current key");

      Console.WriteLine("\n");

      Console.WriteLine("(7) CryptoSetup –rsatest -keyclass DatabasePassword -server engdemo-1 -pfx C:\\temp\\test.pfx -pwd pa$$w0rd");
      Console.WriteLine("    Test that encryption or hashing can be performed for the specified KMS server for a specific key class");

      Console.WriteLine("\n");

      Console.WriteLine("The following are valid key class names:");

      foreach (string keyClassName in CryptoManager.CryptKeyClassNames)
      {
        Console.WriteLine(String.Format("  {0}", keyClassName));
      }

      foreach (string keyClassName in CryptoManager.HashKeyClassNames)
      {
        Console.WriteLine(String.Format("  {0}", keyClassName));
      }
    }

    #region Data
    public static readonly Logger logger = new Logger("[MetraTech.Security.CryptoSetup]");

    #endregion
  }

  /// <summary>
  ///   Abstract command class
  /// </summary>
  abstract class Command
  {
    #region Public Methods
    public Command(string[] args, bool needSfInitialize = false)
    {
      this.args = args;
      msCryptoManager = new MSCryptoManager(needSfInitialize);
    }
    public abstract void Parse();
    public abstract void Execute();

    public string GetError()
    {
      return error;
    }
    #endregion

    #region Properties
    private string error;
    protected string Error
    {
      get { return error; }
      set { error = value; }
    }

    public bool HasErrors
    {
      get
      {
        if (String.IsNullOrEmpty(error))
        {
          return false;
        }

        return true;
      }
    }

    private string output;
    public string Output
    {
      get { return output; }
      set { output = value; }
    }

    protected void HandleError(Exception e)
    {
      Error = e.Message;
      CryptoSetup.logger.LogException("CryptoSetup failed", e);
    }
    #endregion

    #region Data
    protected MSCryptoManager msCryptoManager;
    protected string[] args;
    #endregion
  }
  /// <summary>
  ///   Handle the following CryptoSetup commands
  ///   
  ///   CryptoSetup -createkeys
  ///  
  ///   Create RMP\config\security\sessionkeys.xml. 
  ///   Backup the original sessionkeys.xml to sessionkeys.xml.[Date].old
  /// </summary>
  class CreateKeysCommand : Command
  {
    public CreateKeysCommand(string[] args)
      : base(args)
    {
    }

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 1)
      {
        Error = "Invalid or extra arguments";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        MSSessionKeyConfig sessionKeyConfig = MSSessionKeyConfig.GetInstance();
        if (!String.IsNullOrEmpty(sessionKeyConfig.KeyFile))
        {
          // Backup sessionkeys.xml to sessionkeys.xml.[Date].old, if it exists
          if (File.Exists(sessionKeyConfig.KeyFile))
          {
            string timeStamp = DateTime.Now.ToString("s").Replace(":", "-");
            File.Copy(sessionKeyConfig.KeyFile, sessionKeyConfig.KeyFile.Replace(".xml", ".xml." + timeStamp + ".old"));
          }
        }
        msCryptoManager.CreateSessionKeys(String.Empty);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -encryptpwd
  /// </summary>
  class EncryptConfigCommand : Command
  {
    public EncryptConfigCommand(string[] args)
      : base(args)
    {
    }

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 1)
      {
        Error = "Invalid or extra arguments";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        CryptoManager crypto = new CryptoManager();
        crypto.EncryptPasswords();
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }
  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -dpde (or -dpapidecrypt) someTextEncryptedWithDPAPI
  /// </summary>
  class DPAPIDecryptCommand : Command
  {
    public DPAPIDecryptCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string cipherText;
    public string CipherText
    {
      get { return cipherText; }
      set { cipherText = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 2)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      cipherText = args[1].Trim();

      if (String.IsNullOrEmpty(cipherText))
      {
        Error = "CipherText not specified";
      }
    }

    public override void Execute()
    {
      try
      {
        Output = DPAPIWrapper.Decrypt(cipherText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -dpen (or -dpapiencrypt) plainText
  /// </summary>
  class DPAPIEncryptCommand : Command
  {
    public DPAPIEncryptCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string plainText;
    public string PlainText
    {
      get { return plainText; }
      set { plainText = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 2)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      plainText = args[1].Trim();

      if (String.IsNullOrEmpty(plainText))
      {
        Error = "Plain text not specified";
      }
    }

    public override void Execute()
    {
      try
      {
        Output = DPAPIWrapper.Encrypt(plainText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -msde (or -msdecrypt) someTextEncryptedWithMS
  /// </summary>
  class MSDecryptCommand : Command
  {
    public MSDecryptCommand(string[] args, bool needSfInitialize = false)
      : base(args, needSfInitialize)
    {
    }

    #region Properties
    private string cipherText;
    public string CipherText
    {
      get { return cipherText; }
      set { cipherText = value; }
    }

    private CryptKeyClass keyClass;
    public CryptKeyClass KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 2)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      cipherText = args[1].Trim();

      if (String.IsNullOrEmpty(cipherText))
      {
        Error = "CipherText not specified";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        MSCryptoManager cryptoManager = new MSCryptoManager();
        // The keyclass doesn't matter, the key on cipherText will be used to retrieve the key class
        Output = cryptoManager.Decrypt(CryptKeyClass.DatabasePassword, cipherText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -msen (or msencrypt) plainText -keyclass DatabasePassword
  /// </summary>
  class MSEncryptCommand : Command
  {
    public MSEncryptCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string plainText;
    public string PlainText
    {
      get { return plainText; }
      set { plainText = value; }
    }

    private CryptKeyClass keyClass;
    public CryptKeyClass KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 4)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      plainText = args[1].Trim();

      if (String.IsNullOrEmpty(plainText))
      {
        Error = "PlainText not specified";
        return;
      }

      if (args[2].Trim().ToLower() != "-keyclass")
      {
        Error = "Expected -keyclass";
        return;
      }

      string keyClassName = args[3].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClassName))
      {
        Error = "Invalid key class specified";
        return;
      }

      object keyClassEnum = null;

      try
      {
        keyClassEnum = Enum.Parse(typeof(CryptKeyClass), keyClassName);
      }
      catch (Exception)
      {
        keyClassEnum = null;
      }

      if (keyClassEnum == null)
      {
        Error = String.Format("The key class value '{0}' is not a member of CryptKeyClass", keyClassName);
        return;
      }
      else
      {
        keyClass = (CryptKeyClass)keyClassEnum;
      }
    }

    public override void Execute()
    {
      try
      {
        MSCryptoManager cryptoManager = new MSCryptoManager();
        Output = cryptoManager.Encrypt(keyClass, plainText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -rsade (or -rsadecrypt) someTextEncryptedWithRSA -keyclass DatabasePassword
  /// </summary>
  class RSADecryptCommand : Command
  {
    public RSADecryptCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string cipherText;
    public string CipherText
    {
      get { return cipherText; }
      set { cipherText = value; }
    }

    private CryptKeyClass keyClass;
    public CryptKeyClass KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 4)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      cipherText = args[1].Trim();

      if (String.IsNullOrEmpty(cipherText))
      {
        Error = "CipherText not specified";
        return;
      }

      if (args[2].Trim().ToLower() != "-keyclass")
      {
        Error = "Expected -keyclass";
        return;
      }

      string keyClassName = args[3].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClassName))
      {
        Error = "Invalid key class specified";
        return;
      }

      object keyClassEnum = null;

      try
      {
        keyClassEnum = Enum.Parse(typeof(CryptKeyClass), keyClassName);
      }
      catch (Exception)
      {
        keyClassEnum = null;
      }

      if (keyClassEnum == null)
      {
        Error = String.Format("The key class value '{0}' is not a member of CryptKeyClass", keyClassName);
        return;
      }
      else
      {
        keyClass = (CryptKeyClass)keyClassEnum;
      }
    }

    public override void Execute()
    {
      try
      {
        RSACryptoManager cryptoManager = new RSACryptoManager();
        Output = cryptoManager.Decrypt(keyClass, cipherText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -rsaen (or rsaencrypt) plainText -keyclass DatabasePassword
  /// </summary>
  class RSAEncryptCommand : Command
  {
    public RSAEncryptCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string plainText;
    public string PlainText
    {
      get { return plainText; }
      set { plainText = value; }
    }

    private CryptKeyClass keyClass;
    public CryptKeyClass KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 4)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      plainText = args[1].Trim();

      if (String.IsNullOrEmpty(plainText))
      {
        Error = "PlainText not specified";
        return;
      }

      if (args[2].Trim().ToLower() != "-keyclass")
      {
        Error = "Expected -keyclass";
        return;
      }

      string keyClassName = args[3].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClassName))
      {
        Error = "Invalid key class specified";
        return;
      }

      object keyClassEnum = null;

      try
      {
        keyClassEnum = Enum.Parse(typeof(CryptKeyClass), keyClassName);
      }
      catch (Exception)
      {
        keyClassEnum = null;
      }

      if (keyClassEnum == null)
      {
        Error = String.Format("The key class value '{0}' is not a member of CryptKeyClass", keyClassName);
        return;
      }
      else
      {
        keyClass = (CryptKeyClass)keyClassEnum;
      }
    }

    public override void Execute()
    {
      try
      {
        RSACryptoManager cryptoManager = new RSACryptoManager();
        Output = cryptoManager.Encrypt(keyClass, plainText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -pwdhash plainText -u username -n namespace
  /// </summary>
  class PasswordHash : Command
  {
    public PasswordHash(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string plainText;
    public string PlainText
    {
      get { return plainText; }
      set { plainText = value; }
    }

    private string userName;
    public string UserName
    {
      get { return userName; }
      set { userName = value; }
    }

    private string nameSpace;
    public string NameSpace
    {
      get { return nameSpace; }
      set { nameSpace = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 6)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      plainText = args[1].Trim();

      if (String.IsNullOrEmpty(plainText))
      {
        Error = "PlainText not specified";
        return;
      }

      if (args[2].Trim().ToLower() != "-u")
      {
        Error = "Expected -u for username";
        return;
      }

      userName = args[3].Trim();
      if (String.IsNullOrEmpty(userName))
      {
        Error = "userName not specified";
        return;
      }

      if (args[4].Trim().ToLower() != "-n")
      {
        Error = "Expected -n for namespace";
        return;
      }

      nameSpace = args[5].Trim();
      if (String.IsNullOrEmpty(nameSpace))
      {
        Error = "namespace not specified";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        Auth auth = new Auth();
        auth.Initialize(userName, nameSpace);
        Output = auth.HashNewPassword(PlainText);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }
  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup –keyclass DatabasePassword –pwd pa$$w0rd 
  /// </summary>
  class KeyClassCommand : Command
  {
    public KeyClassCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string password;
    public string Password
    {
      get { return password; }
      set { password = value; }
    }

    private string keyClass;
    public string KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 4)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      keyClass = args[1].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClass))
      {
        Error = "Invalid key class specified";
        return;
      }

      if (args[2].Trim().ToLower() != "-pwd")
      {
        Error = "Expected -pwd";
        return;
      }

      password = args[3].Trim();
      if (String.IsNullOrEmpty(password))
      {
        Error = "password not specified";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        MSCryptoManager cryptoManager = new MSCryptoManager();
        cryptoManager.CreateKey(keyClass, password);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup –addkey 
  ///               –keyclass DatabasePassword 
  ///               –pwd pa$$w0rd 
  ///               –id cbb23d8d-8189-4039-a575-a7388c369d99 
  ///               [-makecurrent]
  /// </summary>
  class AddKeyCommand : Command
  {
    public AddKeyCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string keyClass;
    public string KeyClass
    {
      get { return keyClass; }
      set { keyClass = value; }
    }

    private string password;
    public string Password
    {
      get { return password; }
      set { password = value; }
    }

    private Guid id;
    public Guid Id
    {
      get { return id; }
      set { id = value; }
    }

    private bool makeCurrent;
    public bool MakeCurrent
    {
      get { return makeCurrent; }
      set { makeCurrent = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 7 && args.Length != 8)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      if (args[1].Trim().ToLower() != "-keyclass")
      {
        Error = "Expected -keyclass";
        return;
      }

      keyClass = args[2].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClass))
      {
        Error = "Invalid key class specified";
        return;
      }

      if (args[3].Trim().ToLower() != "-pwd")
      {
        Error = "Expected -pwd";
        return;
      }

      password = args[4].Trim();
      if (String.IsNullOrEmpty(password))
      {
        Error = "password not specified";
        return;
      }

      if (args[5].Trim().ToLower() != "-id")
      {
        Error = "Expected -id";
        return;
      }

      string identifier = args[6].Trim();
      if (String.IsNullOrEmpty(identifier))
      {
        Error = "Id not specified";
        return;
      }

      id = new Guid(identifier);

      if (args.Length == 8)
      {
        if (args[7].Trim().ToLower() != "-makecurrent")
        {
          Error = "Expected -makecurrent";
          return;
        }
        else
        {
          makeCurrent = true;
        }
      }
    }

    public override void Execute()
    {
      try
      {
        MSCryptoManager cryptoManager = new MSCryptoManager();
        cryptoManager.AddKey(keyClass, password, id, makeCurrent);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }
    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup –current –id cbb23d8d-8189-4039-a575-a7388c369d99 
  ///              
  /// </summary>
  class CurrentKeyCommand : Command
  {
    public CurrentKeyCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private Guid id;
    public Guid Id
    {
      get { return id; }
      set { id = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 3)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      if (args[1].Trim().ToLower() != "-id")
      {
        Error = "Expected -id";
        return;
      }

      string identifier = args[2].Trim();
      if (String.IsNullOrEmpty(identifier))
      {
        Error = "Id not specified";
        return;
      }

      id = new Guid(identifier);
    }

    public override void Execute()
    {
      try
      {
        MSCryptoManager cryptoManager = new MSCryptoManager();
        cryptoManager.MakeKeyCurrent(id);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -pwd pa$$w0rd
  /// </summary>
  class PasswordCommand : Command
  {
    #region Public Methods
    public PasswordCommand(string[] args)
      : base(args)
    {
    }
    #endregion

    #region Properties
    private string password;
    public string Password
    {
      get { return password; }
      set { password = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 2)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      password = args[1].Trim();

      if (String.IsNullOrEmpty(password))
      {
        Error = "Password not specified";
      }
    }

    public override void Execute()
    {
      try
      {
        msCryptoManager.CreateSessionKeys(password);
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }


    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -kmstest -keyclass DatabasePassword -server engdemo-1 -pfx C:\temp\test.pfx -pwd pa$$w0rd
  /// </summary>
  class RSATestCommand : Command
  {
    public RSATestCommand(string[] args)
      : base(args)
    {
    }

    #region Properties
    private string keyClassName;
    public string KeyClassName
    {
      get { return keyClassName; }
      set { keyClassName = value; }
    }

    private string server;
    public string Server
    {
      get { return server; }
      set { server = value; }
    }

    private string certificate;
    public string Certificate
    {
      get { return certificate; }
      set { certificate = value; }
    }

    private string password;
    public string Password
    {
      get { return password; }
      set { password = value; }
    }

    #endregion

    #region ICommand implementation
    public override void Parse()
    {
      if (args.Length != 9)
      {
        Error = "Invalid or extra arguments";
        return;
      }

      if (args[1].Trim().ToLower() != "-keyclass")
      {
        Error = "Expected -keyclass";
        return;
      }

      // Get KeyClass
      keyClassName = args[2].Trim();
      if (!CryptoManager.IsKeyClassNameValid(keyClassName))
      {
        Error = "Invalid key class specified";
        return;
      }

      if (args[3].Trim().ToLower() != "-server")
      {
        Error = "Expected -server";
        return;
      }

      // Get server
      server = args[4].Trim();
      if (String.IsNullOrEmpty(server))
      {
        Error = "Server not specified";
        return;
      }

      if (args[5].Trim().ToLower() != "-pfx")
      {
        Error = "Expected -pfx";
        return;
      }

      // Get server
      certificate = args[6].Trim();
      if (String.IsNullOrEmpty(certificate))
      {
        Error = "Certificate file not specified";
        return;
      }

      if (!File.Exists(certificate))
      {
        Error = "Certificate file not found";
        return;
      }

      if (args[7].Trim().ToLower() != "-pwd")
      {
        Error = "Expected -pwd";
        return;
      }

      password = args[8].Trim();
      if (String.IsNullOrEmpty(password))
      {
        Error = "Password not specified";
        return;
      }
    }

    public override void Execute()
    {
      try
      {
        RSACryptoManager cryptoManager = new RSACryptoManager();
        string configFile = cryptoManager.CreateConfigFile(certificate, server);

        cryptoManager.RSAConfig.KmsCertificatePwd.Password = DPAPIWrapper.Encrypt(password);
        cryptoManager.RSAConfig.KmsCertificatePwd.Encrypted = true;
        cryptoManager.RSAConfig.KmsClientConfigFile = configFile;

        if (CryptoManager.IsCryptKeyClassName(keyClassName))
        {
          CryptKeyClass keyClass = (CryptKeyClass)Enum.Parse(typeof(CryptKeyClass), keyClassName);
          TestCryptKeyClass(cryptoManager, keyClass);
        }
        else
        {
          HashKeyClass keyClass = (HashKeyClass)Enum.Parse(typeof(HashKeyClass), keyClassName);
          TestHashKeyClass(cryptoManager, keyClass);
        }
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    private void TestCryptKeyClass(RSACryptoManager cryptoManager,
                     CryptKeyClass keyClass)
    {
      string cipherText = cryptoManager.Encrypt(keyClass, plainText);
      string decryptedText = cryptoManager.Decrypt(keyClass, cipherText);

      if (decryptedText != plainText)
      {
        Error =
          String.Format("Encryption failed for key class '{0}', server '{1}', certificate '{2}' and password '{3}'",
                keyClass.ToString(),
                server,
                certificate,
                password);

        return;
      }

      Output =
        String.Format("Successfully tested encryption/decryption for key class '{0}', server '{1}', certificate '{2}' and password '{3}'",
               keyClass.ToString(),
               server,
               certificate,
               password);

    }

    private void TestHashKeyClass(RSACryptoManager cryptoManager,
                    HashKeyClass keyClass)
    {
      string cipherText1 = cryptoManager.Hash(keyClass, plainText);
      string cipherText2 = cryptoManager.Hash(keyClass, plainText);

      if (cipherText1 != cipherText2)
      {
        Error =
          String.Format("HMAC failed for key class '{0}', server '{1}', certificate '{2}' and password '{3}'",
                keyClass.ToString(),
                server,
                certificate,
                password);

        return;
      }

      Output =
        String.Format("Successfully tested HMAC for key class '{0}', server '{1}', certificate '{2}' and password '{3}'",
               keyClass.ToString(),
               server,
               certificate,
               password);

    }

    #endregion

    #region Data
    private const string plainText = "The old brown fox jumped over the #$%^()&#@,.:\"//\'|}{[]?_-* moon!";
    #endregion
  }

  /// <summary>
  ///   Handle the following CryptoSetup commands
  /// 
  ///   CryptoSetup -test
  ///   Test each of the key classes in the system using the settings from RMP\config\security\mtsecurity.xml
  /// </summary>
  class TestCommand : Command
  {
    public TestCommand(string[] args)
      : base(args)
    {
    }

    #region ICommand implementation
    public override void Parse()
    {
    }

    public override void Execute()
    {
      try
      {
        #region Print Config Info
        Output += Environment.NewLine;
        Output += "Configuration." + Environment.NewLine;
        Output += "--------------" + Environment.NewLine;
        Output += Environment.NewLine;

        CryptoConfig cryptoConfig = CryptoConfig.GetInstance();
        if (String.IsNullOrEmpty(cryptoConfig.CryptoTypeName))
        {
          throw new ApplicationException(
            String.Format("Missing value for <cryptoTypeName> element in RMP\\config\\security\\mtsecurity.xml."));
        }

        StringBuilder output = new StringBuilder();
        if (cryptoConfig.CryptoTypeName.EndsWith("RSACryptoManager"))
        {
          Output += "Security Provider  : RSA" + Environment.NewLine;

          // Check that the kmsCertificatePwd has been specified
          if (String.IsNullOrEmpty(cryptoConfig.RSAConfig.KmsCertificatePwd.Password))
          {
            throw new ApplicationException(
              String.Format("Missing value for <kmsCertificatePwd> element in RMP\\config\\security\\mtsecurity.xml."));
          }

          List<string> validationErrors;
          KmsClientConfig kmsClientConfig;
          if (!RSAConfig.ValidateKmsClientConfigFile(cryptoConfig.RSAConfig.KmsClientConfigFile, out validationErrors, out kmsClientConfig))
          {
            foreach (string validationError in validationErrors)
            {
              Output += "Error: " + validationError + Environment.NewLine;
            }
            throw new ApplicationException(String.Format("Failed to validate KMS client config file '{0}'", cryptoConfig.RSAConfig.KmsClientConfigFile));
          }

          Output += "Server             : " + kmsClientConfig.Server + Environment.NewLine;
          Output += "Server Port        : " + kmsClientConfig.ServerPort + Environment.NewLine;
          Output += "Client Certificate : " + kmsClientConfig.CertificateFile + Environment.NewLine;
          Output += "Log File           : " + kmsClientConfig.LogFile + Environment.NewLine;
          Output += "Log Level          : " + kmsClientConfig.LogLevel + Environment.NewLine;
          Output += "SSL Connect Timeout: " + kmsClientConfig.ConnectTimeout + Environment.NewLine;
          Output += "SSL Retries        : " + kmsClientConfig.Retries + Environment.NewLine;
          Output += "SSL Retry Delay    : " + kmsClientConfig.RetryDelay + Environment.NewLine;
          Output += "Cache file         : " + kmsClientConfig.CacheFile + Environment.NewLine;
        }
        else if (cryptoConfig.CryptoTypeName.EndsWith("MSCryptoManager"))
        {
          Output += "Security Provider: Microsoft" + Environment.NewLine;
          MSSessionKeyConfig sessionKeyConfig = MSSessionKeyConfig.GetInstance();
          if (!File.Exists(sessionKeyConfig.KeyFile))
          {
            throw new ApplicationException(String.Format("Cannot find session key file '{0}'", sessionKeyConfig.KeyFile));
          }
        }
        else
        {
          throw new ApplicationException(
            String.Format("Invalid <cryptoTypeName> '{0}' found in RMP\\config\\security\\mtsecurity.xml.",
                  cryptoConfig.CryptoTypeName));
        }
        #endregion

        #region Test Encryption/Decryption

        CryptoManager cryptoManager = new CryptoManager();

        Output += Environment.NewLine;
        Output += Environment.NewLine;
        Output += "Testing key classes for encryption." + Environment.NewLine;
        Output += "-----------------------------------" + Environment.NewLine;
        Output += Environment.NewLine;

        const string plainText = "日本語 The old brown fox jumped over the #$%^()&#@,.:\"//\'|}{[]?_-* moon!";

        foreach (CryptKeyClass cryptKeyClass in Enum.GetValues(typeof(CryptKeyClass)))
        {
          try
          {
            string cipher = cryptoManager.Encrypt(cryptKeyClass, plainText);
            string decryptedText = cryptoManager.Decrypt(cryptKeyClass, cipher);
            if (plainText == decryptedText)
            {
              Output += "Key Class '" + cryptKeyClass + "' tested successfully." + Environment.NewLine;
            }
            else
            {
              Output += "Key Class '" + cryptKeyClass + "' test FAILED!" + Environment.NewLine;
            }
          }
          catch (Exception e)
          {
            Output += "Key Class '" + cryptKeyClass + "' test FAILED!" + Environment.NewLine;
            Output += "Error message: " + e.Message + Environment.NewLine;
            Output += "Stack Trace: " + e.StackTrace + Environment.NewLine;
            Output += Environment.NewLine;
          }
        }
        #endregion

        #region Test Hashing
        Output += Environment.NewLine;
        Output += Environment.NewLine;
        Output += "Testing key classes for hashing." + Environment.NewLine;
        Output += "--------------------------------" + Environment.NewLine;
        Output += Environment.NewLine;


        foreach (HashKeyClass hashKeyClass in Enum.GetValues(typeof(HashKeyClass)))
        {
          try
          {
            string hash1 = cryptoManager.Hash(hashKeyClass, plainText);
            string hash2 = cryptoManager.Hash(hashKeyClass, plainText);
            if (hash1 == hash2)
            {
              Output += "Key Class '" + hashKeyClass.ToString() + "' tested successfully." + Environment.NewLine;
            }
            else
            {
              Output += "Key Class '" + hashKeyClass.ToString() + "' test FAILED!" + Environment.NewLine;
            }
          }
          catch (Exception e)
          {
            Output += "Key Class '" + hashKeyClass + "' test FAILED!" + Environment.NewLine;
            Output += "Error message: " + e.Message + Environment.NewLine;
            Output += "Stack Trace: " + e.StackTrace + Environment.NewLine;
            Output += Environment.NewLine;
          }
        }

        #endregion
      }
      catch (Exception e)
      {
        HandleError(e);
        throw;
      }

    }

    #endregion

    #region Data
    private IMTRcd rcd = new MTRcdClass();
    #endregion
  }
}
