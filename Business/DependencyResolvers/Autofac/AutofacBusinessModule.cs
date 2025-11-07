using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Core.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace Business.DependencyResolvers.Autofac
{
   public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CarManager>().As<ICarService>().SingleInstance();
            builder.RegisterType<EfCarDal>().As<ICarDal>().SingleInstance();

            builder.RegisterType<BrandManager>().As<IBrandService>().SingleInstance();
            builder.RegisterType<EfBrandDal>().As<IBrandDal>().SingleInstance();

            builder.RegisterType<ColorManager>().As<IColorService>().SingleInstance();
            builder.RegisterType<EfColorDal>().As<IColorDal>().SingleInstance();

            //builder.RegisterType<IndividualCustomerManager>().As<IIndividualCustomerService>().SingleInstance();
            //builder.RegisterType<EfIndividualCustomerDal>().As<IIndividualCustomerDal>().SingleInstance();

            //builder.RegisterType<CorporateCustomerManager>().As<ICorporateCustomerService>().SingleInstance();
            //builder.RegisterType<EfCorporateCustomerDal>().As<ICorporateCustomerDal>().SingleInstance();

            builder.RegisterType<IndividualCustomerManager>().As<IIndividualCustomerService>().SingleInstance();
            builder.RegisterType<EfIndividualCustomerDal>().As<IIndividualCustomerDal>().SingleInstance();

            builder.RegisterType<CorporateCustomerManager>().As<ICorporateCustomerService>().SingleInstance();
            builder.RegisterType<EfCorporateCustomerDal>().As<ICorporateCustomerDal>().SingleInstance();

            builder.RegisterType<FuelManager>().As<IFuelService>().SingleInstance();
            builder.RegisterType<EfFuelDal>().As<IFuelDal>().SingleInstance();

            builder.RegisterType<GearManager>().As<IGearService>().SingleInstance();
            builder.RegisterType<EfGearDal>().As<IGearDal>().SingleInstance();

            builder.RegisterType<SegmentManager>().As<ISegmentService>().SingleInstance();
            builder.RegisterType<EfSegmentDal>().As<ISegmentDal>().SingleInstance();

            builder.RegisterType<RentalManager>().As<IRentalService>().SingleInstance();
            builder.RegisterType<EfRentalDal>().As<IRentalDal>().SingleInstance();

            builder.RegisterType<CarImageManager>().As<ICarImageService>().SingleInstance();
            builder.RegisterType<EfCarImageDal>().As<ICarImageDal>().SingleInstance();

             builder.RegisterType<FileHeplerManager>().As<IFileHelper>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<CorporateUserManager>().As<ICorporateUserService>();
            builder.RegisterType<EfCorporateUserDal>().As<ICorporateUserDal>();

            builder.RegisterType<LocationManager>().As<ILocationService>();
            builder.RegisterType<EfLocationDal>().As<ILocationDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<LocationOperationClaimManager>().As<ILocationOperationClaimService>();
            builder.RegisterType<EfLocationOperationClaimDal>().As<ILocationOperationClaimDal>();

            builder.RegisterType<PricingManager>().As<IPricingService>();

            builder.Register(c => new HttpClient()).As<HttpClient>().SingleInstance();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
