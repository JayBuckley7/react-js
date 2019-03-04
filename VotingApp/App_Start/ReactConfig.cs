using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VotingApp.ReactConfig), "Configure")]

namespace VotingApp
{
    /// <summary>
    /// this comes with react, i didnt change it at all
    /// </summary>
	public static class ReactConfig
	{
		public static void Configure()
		{

		}
	}
}