using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [HideMonoScript]
    public abstract class EMS_Coliider_Abstract_LinkList<ColiiderLink> : EMS_Abstract_LinkList<ColiiderLink> where ColiiderLink:ILink_EMS_Collider
    {

    }
    [HideMonoScript]
    public abstract class EMS_Coliider2D_Abstract_LinkList<Coliider2DLink> : EMS_Abstract_LinkList<Coliider2DLink> where Coliider2DLink : ILink_EMS_Collider2D
    {

    }
    [HideMonoScript]
    public abstract class EMS_Coliider_Abstract_LinkSingle<ColiiderLink> : EMS_Abstract_LinkSingle<ColiiderLink> where ColiiderLink : ILink_EMS_Collider
    {

    }
    [HideMonoScript]
    public abstract class EMS_Coliider2D_Abstract_LinkSingle<Coliider2DLink> : EMS_Abstract_LinkSingle<Coliider2DLink> where Coliider2DLink : ILink_EMS_Collider2D
    {

    }
}