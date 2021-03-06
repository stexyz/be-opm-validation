﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using opm_validation_service.Models;

namespace opm_validation_service.Persistence
{
    public static class OpmRepoFiller
    {
        [Obsolete]
        private static readonly string[] Input = new[]
            {
                "859182400204379570", "859182400210288910", "859182400206514634", "859182400204081893",
                "859182400205312606", "859182400209851101", "859182400206328927", "859182400207026921",
                "859182400204314298", "859182400205108803", "859182400704103088", "859182400204236002",
                "859182400402426168", "859182400203921794", "859182400205307848", "859182400100447106",
                "859182400204150193", "859182400207034254", "27ZG100Z0052788P", "859182400891213065",
                "859182400891214925", "859182400891220766", "859182400891222180", "859182400891224009",
                "859182400891267754", "859182400891276794", "859182400892320373", "859182400892324180",
                "859182400892331768", "859182400892333694", "859182400893009819", "859182400893014134"
            };

        public static void Fill(IOpmRepository repo, string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (!repo.TryAdd(new Opm(new EanEicCode(currentLine))))
                    {
                        throw new Exception("Failed to add OPM [code= " + currentLine + "] to repository.");
                    }
                }
            }
        }
    }
}