using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OrnekSite.App_Start.Startup1))]

namespace OrnekSite.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // Uygulamanızı nasıl yapılandıracağınız hakkında daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=316888 adresini ziyaret edin
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            //app parametresi yukarıdan gelmektedir.
            {
                AuthenticationType="ApplicationCookie", LoginPath=new PathString("/Account/Login")
                //owın, OpenWebInterfacefor.NET, .NET sunucusunu birbirinden ayırmak için oluşturulmuş yapıları bulunduran bir intefacedir.
                //OWIN uyumlu hazırlanmış bir uygulama çalıştığı ortamdan bağımszdır.
                //ASP.NET identity membershift sisteminde log in , log out işlemleri için OWİN kullanılır.
                //Cookie yönetimi membershift form authentication ile gerçekleştirilirken OWIN de cookie authentication ile sağlanır.
                //AuthenticationType hangi tip cookie ile işlem yapılacağı belirtilir.
                //UseCookieAuthentication asp.net identity framework'e bunun cookie bazlı bir authentication olduğunu belirtiyor.

                // LoginPath ile kullanıcı yetkisi isteyen bir alana erişmek istendiğinde yönlendirilmesi istediğimiz default alandır.

            });
        }
    }
}
