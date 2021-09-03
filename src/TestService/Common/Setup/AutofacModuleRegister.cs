using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common.Setup
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //注册Service
            //Assembly.Load("[工程名称]");
            var assemblysServices = Assembly.Load("Services");
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerDependency()//瞬时单例
               .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors(); //引用Autofac.Extras.DynamicProxy;

            //注册Repository
            //Assembly.Load("[工程名称]");
            var assemblysRepository = Assembly.Load("Repository");
            builder.RegisterAssemblyTypes(assemblysRepository)
                .InstancePerDependency()//瞬时单例
               .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors(); //引用Autofac.Extras.DynamicProxy;

        }

    }
}
