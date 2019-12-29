using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class InsuredsRepository
    {
        public Insured GetInsuredById(string insuredId)
        {
            Insured insured;
            using (var db = new ApplicationDbContext())
            {
                insured = db.Insureds.Include("InsuredPlan")
                                     .SingleOrDefault(i => i.UserId == insuredId);
            }

            return insured;
        }
    }
}