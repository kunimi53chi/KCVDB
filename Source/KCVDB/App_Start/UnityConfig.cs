using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using KCVDB.Services;
using KCVDB.Services.BlobStorage;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace KCVDB
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();

			// サービスを登録

			// この処理環境ごとに振り分けできるようなクラス作ってそこに任せたい
			var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[Constants.BlobStorage.ApiDataStorageKey].ConnectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(Constants.BlobStorage.ApiDataBlobContainerName);

			container.RegisterType<IApiDataWriter>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AzureBlobApiDataWriter(blobContainer)));
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}