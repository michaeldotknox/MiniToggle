# MiniToggle
A feature toggle library

[![Build status](https://ci.appveyor.com/api/projects/status/nryn2sx789mx1k07/branch/master?svg=true)](https://ci.appveyor.com/project/michaeldotknox/minitoggle/branch/master)

MiniToggle is a feature toggle library using what I consider to be the best implementations of the various feature toggle libraries I have come across.

## What are Feature Toggles
Feature toggles are a way of simplifying the creation of a feature and deploying that feature.  Using feature toggles in a Continuous Deployment environment
can reduce or eliminate the need to create a new feature branch for every feature and therefore avoid the merge issues that will inevitably occur when you
try to merge the feature branch back into the main branch.

With feature toggles, the code for the new feature is hidden behind the toggle, which allows the new feature development to continue without impacting the
existing code.  The advantage of a feature toggle library like MiniToggle is that they reduce the need to write the same code to check to see if a feature
is enabled or not.

In MiniToggle, when you create a toggle, it simply implements the `IToggle` interface.  There is no code that your toggle class needs to implement.  The
toggle can be configured with an attribute on the class, or it can be configured with a fluent interface during initialization.

## Creating toggles in your project
As mentioned above, to create the toggle, you only need to create a class that implements the `IToggle` interface like this:
````
public class MyFeature : IToggle
{
}
````
That's all there is to create the toggle.  The nice thing about this is that you can use CodeLens in Visual Studio to see if the toggle is used, which can
make it easier to clean up unused toggles.

## Configuring your toggles
Once you have created your toggle, you need to configure it so MiniToggle can use it.  There are four built-in toggles with MiniToggle:
* Always True
* Always False
* Settings File
* Delegate

Most of these can be configured using attributes on your toggle class.  To create a toggle that is always enabled, use the always true toggle:
````
[AlwaysTrue]
public class MyFeature : IToggle
{
}
````
If you want the toggle to always be disabled, choose the Always False toggle:
````
[AlwaysFalse]
public class MyFeature : IToggle
{
}
````
If you want the status of the toggle to be determined by the web.config file or the app.config file, use the setting configuration toggle:
````
[SettingConfiguration("MyFeatureConfiguration")]
public class MyFeature : IToggle
{
}
````
This will use a setting called "MyFeatureConfiguration" in the configuration file.  If the value for that setting is "true", then the toggle will be enabled.
If the value is anything else, the toggle will be disabled.
The setting configuration toggle will default to true if the setting in not in the configuration file.  You can override this behavior by using specifying
a default value for the toggle:
````
[SettingConfiguration("MyFeatureConfiguration", false)]
public class MyFeature : IToggle
{
}
````
These three toggles were all configured using attributes on the toggle class.  You can also configure the toggles using the fluent interface.
## Configuring toggles with the fluent interface
All of the toggles can be configured using a fluent interface.  You would do this during initialization of your application.  This is started with the use of the 
'Is' method:
````
Toggle<MyFeature>.Is().AlwaysTrue();
````
There is a corresponding configuration for the always false toggle:
````
Toggle<MyFeature>.Is().AlwaysFalse();
````
To configure a toggle as a setting configuration toggle, add `Configured().WithSetting()`:
````
Toggle<MyFeature>.Is().Configured().WithSetting("MyFeatureConfiguration");
````
To specify a default for the toggle, add the default method at the end:
````
Toggle<MyFeature>.Is().Configured().WithSetting("MyFeatureConfiguration").Default(false);
````
This will cover many of the configurations that you will need, but there may be times when you need to retrieve the value for a toggle from somewhere other than
a configuration file, such as from a database.  This is where the delegate toggle comes in.

This toggle will allow you to specify a delegate that will be called to determine the status of the toggle.  This can be any method on any object as long as the
value returned is a boolean.  This allows you to specify the parameters that your system needs to determine the status of the toggle, whether that is from a
database, or a web service, or some other logic.  It is entirely up to you with this toggle.  For instance:
````
Toggle<MyFeature>.Is().Configured().With().Delegate(() => MyFeatureRepository.GetFeatureSetting("MyFeature"));
````
With the delegate toggle, there may be times that you want to cache the results to prevent calling the delegate every time the status of the delegate is checked.
You can do this by specifying that MiniToggle should cache the status of the toggle with the `Cache` method, like this:
````
Toggle<MyFeature>.Is().Cached().AtInitialization()
````
Any toggles that are cached like this will retrieve their status only one time, during the intialization of the application.
## Getting the status of the toggle
Once you have your toggles defined you need to be able to query the status of the toggle to execute the code accordingly.  To retrieve the status of the toggle, call the `IsEnabled` method:
````
if (Toggle<MyFeature>.IsEnabled())
{
    // ...execute some code
}
else
{
    // ...execute some other code
}
````
While this works, it can lead to a lot of if-then-else statements in your code.  You can reduce the number of if-then-else statements by calling the `Execute` method on the toggle:
````
Toggle<MyFeature>.Execute(
    () => {// Execute this if true},
    () => {// Execute this if false}
);
````
The `Execute` method takes two delegates, one to execute if the toggle is enabled, and the other to execute if the toggle is disabled.  The example above shows the `Execute` method
`Action` parameters, but an overload also takes two `Func<TResult>` parameters and returns the result of the executed delegate.  There is also an `ExecuteAsync` method which accepts
two `Func<Task>` parameters or two `<Func<Task<TResult>>` parameters.
## Ideas always welcome
I am always open to new ideas or problems that may come out of MiniToggle.  Feel free to open an issue with any ideas or concerns.
