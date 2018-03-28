..\.nuget\nuget pack DContainer\DContainer.nuspec -Version 1.0.13 -OutputDirectory DContainer
..\.nuget\nuget pack DContainer.Autofac\DContainer.Autofac.nuspec -Version 1.0.13 -OutputDirectory DContainer.Autofac
..\.nuget\nuget pack DContainer.Castle\DContainer.Castle.nuspec -Version 1.0.12 -OutputDirectory DContainer.Castle
..\.nuget\nuget pack DContainer.Spring\DContainer.Spring.nuspec -Version 1.0.13 -OutputDirectory DContainer.Spring
..\.nuget\nuget pack DContainer.Unity\DContainer.Unity.nuspec -Version 1.0.12 -OutputDirectory DContainer.Unity
..\.nuget\nuget pack DContainer.Mvc\DContainer.Mvc.nuspec -Version 1.0.16 -OutputDirectory DContainer.Mvc
..\.nuget\nuget pack DContainer.WebApi\DContainer.WebApi.nuspec -Version 1.0.3 -OutputDirectory DContainer.WebApi
..\.nuget\nuget pack DContainer.Prism\DContainer.Prism.nuspec -Version 1.0.11 -OutputDirectory DContainer.Prism
..\.nuget\nuget pack DContainer.ServiceModel\DContainer.ServiceModel.nuspec -Version 1.0.11 -OutputDirectory DContainer.ServiceModel
pause

..\.nuget\nuget push DContainer\DContainer.1.0.13.nupkg
..\.nuget\nuget push DContainer.Autofac\DContainer.Autofac.1.0.13.nupkg
..\.nuget\nuget push DContainer.Castle\DContainer.Castle.1.0.12.nupkg
..\.nuget\nuget push DContainer.Spring\DContainer.Spring.1.0.13.nupkg
..\.nuget\nuget push DContainer.Unity\DContainer.Unity.1.0.12.nupkg
..\.nuget\nuget push DContainer.Mvc\DContainer.Mvc.1.0.16.nupkg
..\.nuget\nuget push DContainer.WebApi\DContainer.WebApi.1.0.3.nupkg
..\.nuget\nuget push DContainer.Prism\DContainer.Prism.1.0.11.nupkg
..\.nuget\nuget push DContainer.ServiceModel\DContainer.ServiceModel.1.0.11.nupkg
pause



