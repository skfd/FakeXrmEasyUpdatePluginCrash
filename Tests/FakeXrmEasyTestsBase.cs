using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Middleware;
using Microsoft.Xrm.Sdk;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Middleware.Pipeline;

public class FakeXrmEasyTestsBase
{
    protected readonly IXrmFakedContext _context;
    protected readonly IOrganizationService _service;

    public FakeXrmEasyTestsBase()
    {
        _context = MiddlewareBuilder
                        .New()
                        .AddCrud()
                        .AddFakeMessageExecutors()
                        .AddPipelineSimulation()

                        .UsePipelineSimulation()
                        .UseCrud()
                        .UseMessages()

                        .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                        .Build();

        _service = _context.GetOrganizationService();
    }
}
