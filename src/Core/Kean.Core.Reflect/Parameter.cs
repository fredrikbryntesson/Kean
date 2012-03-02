
namespace Kean.Core.Reflect
{
	public class Parameter
	{
		public Type Type { get { return this.information.ParameterType; } }
		System.Reflection.ParameterInfo information;
		internal Parameter(Method method, System.Reflection.ParameterInfo information)
		{
			this.information = information;
		}
	}
}
