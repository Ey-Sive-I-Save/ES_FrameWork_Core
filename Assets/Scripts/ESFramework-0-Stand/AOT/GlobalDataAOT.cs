using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ES.GlobalDataStand;
namespace ES
{
    public class GlobalDataSupportAOT
    {
        void AOT()
        {
            _ = new GlobalDataSupportStand<GlobalDataForResMaster>();
            _ = new GlobalDataSupportStand<GlobalDataForEditorOnly>();
        }
    }
}
