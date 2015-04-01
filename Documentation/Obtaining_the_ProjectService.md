Obtaining the ProjectService
============================

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by "new"ing
up an instance of your object).

    [Import]
    ProjectService ProjectService { get; set; }

### From MEF via an imperative GetService query

    IServiceProvider site; // the VS global service provider
    var componentModel = site.GetService(typeof(SComponentModel)) as IComponentModel;
    var projectServiceAccessor = componentModel.GetService<IProjectServiceAccessor>();
    ProjectService projectService = projectServiceAccessor.GetProjectService();
