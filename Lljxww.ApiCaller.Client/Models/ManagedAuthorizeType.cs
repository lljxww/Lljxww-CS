using System;
using System.Collections.ObjectModel;
using Lljxww.ApiCaller.Models.Config;

namespace Lljxww.ApiCaller.Client.Models
{
	public class ManagedAuthorizeType
	{
        public ObservableCollection<Authorization> Auths { get; set; }
            = new ObservableCollection<Authorization>();

        public ManagedAuthorizeType(ManagedApiCallerConfig config)
        {
            if(config == null)
            {
                return;
            }

            if(config.Config.Authorizations == null)
            {
                return;
            }

            foreach(var auth in config.Config.Authorizations)
            {
                Auths.Add(auth);
            }
        }
    }
}

