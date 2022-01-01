using System.Collections.Generic;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Models.Customers
{
    public readonly struct CustomerCompanyInfo
    {
        public CustomerCompanyInfo(int id, string name, bool isMaster, List<InCompanyPermissions> inCompanyPermissions)
        {
            Id = id;
            Name = name;
            IsMaster = isMaster;
            InCompanyPermissions = inCompanyPermissions;
        }


        /// <summary>
        ///     Id of the company.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Name of the company.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Flag indicating that customer is master in this company.
        /// </summary>
        public bool IsMaster { get; }

        /// <summary>
        ///     List of permissions in current company.
        /// </summary>
        public List<InCompanyPermissions> InCompanyPermissions { get; }
    }
}