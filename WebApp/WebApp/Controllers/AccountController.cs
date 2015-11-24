using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Properties;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private MongoClient mongoClient;
        private IMongoDatabase KonradRequirementsDatabase;
        private IMongoCollection<BsonDocument> usersCollection;

        public AccountController()
        {
            mongoClient = new MongoClient(Settings.Default.MongoDBConnectionString);

            KonradRequirementsDatabase = mongoClient.GetDatabase("KonradRequirements");
            usersCollection = KonradRequirementsDatabase.GetCollection<BsonDocument>("Users");
        }

        // GET: Account
        public ActionResult MyProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditPhoto(HttpPostedFileBase profilePhotoUser, string usernamePhoto)
        {
            if (profilePhotoUser != null)
            {
                if (profilePhotoUser.ContentLength > 0)
                {
                    GridFSBucketOptions bucketOptions = new GridFSBucketOptions() { BucketName = "ProfileImages" };
                    var fs = new GridFSBucket(KonradRequirementsDatabase, bucketOptions);

                    // getting filename
                    string fileName = Path.GetFileName(profilePhotoUser.FileName);

                    //get the bytes from the content stream of the file
                    byte[] pictureBytes = new byte[profilePhotoUser.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(profilePhotoUser.InputStream))
                    {
                        pictureBytes = theReader.ReadBytes(profilePhotoUser.ContentLength);
                    }
                    
                    // Saving picture
                    var objectId = await fs.UploadFromBytesAsync(fileName, pictureBytes);

                    var filter = Builders<BsonDocument>.Filter.Eq("Username", usernamePhoto);

                    var update = Builders<BsonDocument>.Update
                    .Set("ProfileImageName", fileName);

                    var result = await usersCollection.UpdateOneAsync(filter, update);

                    return RedirectToAction("MyProfile", new { ReLogin = true, result = result });
                }
            }
            return null;
        }
    }
}