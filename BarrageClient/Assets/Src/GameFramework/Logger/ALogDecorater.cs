namespace GameFramework
{
	public abstract class ALogDecorater
	{
		private int m_Level;
		protected readonly ALogDecorater m_Decorater;

		protected ALogDecorater(ALogDecorater decorater = null)
		{
			m_Decorater = decorater;
			this.Level = 0;
		}

		protected int Level
		{
			get
			{
				return m_Level;
			}
			set
			{
				m_Level = value;
				if (m_Decorater != null)
				{
					m_Decorater.Level = value + 1;
				}
			}
		}

		public virtual string Decorate(string message)
		{
			if (m_Decorater == null)
			{
				return message;
			}
			return m_Decorater.Decorate(message);
		}
	}
}