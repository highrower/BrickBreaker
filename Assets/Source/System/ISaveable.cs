public interface ISaveable
{
	string ID { get; }

	void Save(SaveData data);

	void Load(SaveData data);
}