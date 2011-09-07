
namespace Kean.Core.Reflect
{
	public class Parameter
	{
		public Type Type { get { return this.parameterInformation.ParameterType; } }
		System.Reflection.ParameterInfo parameterInformation;
		internal Parameter(Method method, System.Reflection.ParameterInfo parameterInformation)
		{
			this.parameterInformation = parameterInformation;
		}
	}
}
