using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Pipeline;
using FakeXrmEasy.Plugins.PluginSteps;
using FakeXrmEasyUpdatePluginCrash;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class Class1: FakeXrmEasyTestsBase
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void FirstRun()
        {
            _context.RegisterPluginStep<MyPlugin>(new PluginStepDefinition()
            {
                MessageName = "Update",
                EntityLogicalName = "quotedetail",
                Stage = ProcessingStepStage.Postoperation,
                FilteringAttributes = new List<string> { "priceperunit", "volumediscountamount" },
            });

            var quote = new Entity("quote", Guid.NewGuid());

            var quoteDetail = new Entity("quotedetail", Guid.NewGuid())
            {
                ["quoteid"] = quote.ToEntityReference(),
                ["priceperunit"] = new Money(123),
            };

            var entities = new List<Entity>() { quoteDetail, quote };
            _context.Initialize(entities);

            var svc = _context.GetOrganizationService();

            var targetQuoteDetail = new Entity("quotedetail", quoteDetail.Id)
            {
                ["priceperunit"] = new Money(4),
            };

            svc.Update(targetQuoteDetail);


            /*
             * System.ArgumentException
              HResult=0x80070057
              Message=GenericArguments[0], 'FakeXrmEasyUpdatePluginCrash.MyPlugin', on 'Microsoft.Xrm.Sdk.IPlugin ExecutePluginWith[T](FakeXrmEasy.Abstractions.IXrmBaseContext, FakeXrmEasy.Plugins.XrmFakedPluginExecutionContext)' violates the constraint of type 'T'.
              Source=mscorlib
              StackTrace:
               at System.RuntimeType.ValidateGenericArguments(MemberInfo definition, RuntimeType[] genericArguments, Exception e)
               at System.Reflection.RuntimeMethodInfo.MakeGenericMethod(Type[] methodInstantiation)
               at FakeXrmEasy.Pipeline.IXrmFakedContextPipelineExtensions.GetPluginMethod(PluginStepDefinition pluginStepDefinition)
               at FakeXrmEasy.Pipeline.IXrmFakedContextPipelineExtensions.ExecutePipelinePlugins(IXrmFakedContext context, IEnumerable`1 pluginSteps, OrganizationRequest organizationRequest, Entity previousValues, Entity resultingAttributes, OrganizationResponse organizationResponse)
               at FakeXrmEasy.Pipeline.IXrmFakedContextPipelineExtensions.ExecutePipelineStage(IXrmFakedContext context, PipelineStageExecutionParameters parameters)
               at FakeXrmEasy.Middleware.Pipeline.MiddlewareBuilderPipelineExtensions.ProcessPostOperation(IXrmFakedContext context, OrganizationRequest request, OrganizationResponse response, Entity preEntity, Entity postEntity)
               at FakeXrmEasy.Middleware.Pipeline.MiddlewareBuilderPipelineExtensions.<>c__DisplayClass4_0.<UsePipelineSimulation>b__1(IXrmFakedContext context, OrganizationRequest request)
               at FakeXrmEasy.Middleware.MiddlewareBuilder.<>c__DisplayClass9_0.<Build>b__2(OrganizationRequest request)
               at FakeItEasy.Configuration.RuleBuilder.ReturnValueConfiguration`1.<>c__DisplayClass28_0.<ReturnsLazily>b__0(IInterceptedFakeObjectCall call)
               at FakeItEasy.Configuration.BuildableCallRule.Apply(IInterceptedFakeObjectCall fakeObjectCall)
               at FakeItEasy.Core.FakeManager.ApplyBestRule(IInterceptedFakeObjectCall fakeObjectCall)
               at FakeItEasy.Core.FakeManager.FakeItEasy.Core.IFakeCallProcessor.Process(InterceptedFakeObjectCall fakeObjectCall)
               at Castle.DynamicProxy.AbstractInvocation.Proceed()
               at Castle.Proxies.ObjectProxy.Execute(OrganizationRequest request)
               at FakeXrmEasy.Middleware.Crud.MiddlewareBuilderCrudExtensions.<>c__DisplayClass7_0.<AddFakeUpdate>b__1(Entity e)
               at FakeItEasy.Configuration.BuildableCallRule.Apply(IInterceptedFakeObjectCall fakeObjectCall)
               at FakeItEasy.Core.FakeManager.ApplyBestRule(IInterceptedFakeObjectCall fakeObjectCall)
               at FakeItEasy.Core.FakeManager.FakeItEasy.Core.IFakeCallProcessor.Process(InterceptedFakeObjectCall fakeObjectCall)
               at Castle.DynamicProxy.AbstractInvocation.Proceed()
               at Tests.Class1.FirstRun() in C:\Users\skfd\Code\Tests\Class1.cs:line 54

              This exception was originally thrown at this call stack:
                [External Code]

            Inner Exception 1:
            VerificationException: Method FakeXrmEasy.Plugins.IXrmBaseContextPluginExtensions.ExecutePluginWith: type argument 'FakeXrmEasyUpdatePluginCrash.MyPlugin' violates the constraint of type parameter 'T'.
             * 
             * 
             */
        }
    }
}
