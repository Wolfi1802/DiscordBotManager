using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCSharp
{
    public class Equipment
    {
        private static Equipment instance = null;
        private static readonly object instanceLock = new object();

        Equipment()
        { }

        public static Equipment GetInstance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new Equipment();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Durchsucht die Datenbank nach dem Equipment und gibt dieses zurück wenn es gefunden wurde ansonsten null!
        /// </summary>
        public Equipment GetEquipmentFromInstance()
        {
            Equipment equipment = new Equipment();
            bool equipmentFounded = false;

            /*
            TODO DB STUFF
            */

            if (equipmentFounded)
                return equipment;
            else
                return null;
        }

    }
}
