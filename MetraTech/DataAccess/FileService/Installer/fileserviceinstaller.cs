namespace MetraTech.FileService
{
  //////////////////////////////////////////
  // Assemblies
  //////////////////////////////////////////
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Configuration.Install;
  //////////////////////////////////////////
  // Interfaces
  //////////////////////////////////////////
  //////////////////////////////////////////
  // Delegates
  //////////////////////////////////////////
  //////////////////////////////////////////
  // Enumerations
  //////////////////////////////////////////
  //////////////////////////////////////////
  // Classes
  //////////////////////////////////////////
  [RunInstaller(true)]
  public partial class cFileLandingServiceInstaller : Installer
  {
    public cFileLandingServiceInstaller()
    {
      InitializeComponent();
    }

    private void svcInstaller_AfterInstall(object sender, InstallEventArgs e)
    {

    }
  }
}