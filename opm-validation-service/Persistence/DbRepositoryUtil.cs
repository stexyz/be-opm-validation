using System.Text;
using opm_validation_service.Persistence.ORM;

namespace opm_validation_service.Persistence
{
    public static class DbRepositoryUtil
    {
        private const string PositiveTestData = "OpmRepoSampleData.csv";

        public static void RecreateDatabase()
        {
            BE_Opm context = new BE_Opm();

            //Force all connections to be closed (to enable DB recreation).
            context.Database.ExecuteSqlCommand("ALTER DATABASE [" + context.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

            if (context.Database.Exists()) {
                context.Database.Delete();
            }
            context.Database.Create();
        }

        public static void FillSampleOpm(string path)
        {
            IOpmRepository repo = new OpmDbRepository();
            OpmRepoFiller.Fill(repo, path);
        }

        public static string GetDbInfo()
        {
            BE_Opm context = new BE_Opm();
            return string.Format("Database:{0}, Data source:{1}.", context.Database.Connection.Database, context.Database.Connection.DataSource);
        }
    }
}