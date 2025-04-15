using System.Globalization;
using Neo4j.Driver;

namespace University_Schedule_Lab1.Repositories;

public class LectureRepository
{
    private readonly IDriver _neo4j;
    private readonly ILogger<LectureRepository> _logger;

    public LectureRepository(IDriver neo4j, ILogger<LectureRepository> logger)
    {
        _neo4j = neo4j;
        _logger = logger;
    }

    public async Task<(List<int> studentIdList, List<int> groupIdList)> GetGroupIdsByLectureId(IEnumerable<int> LectureIds)
    {
        await using var session = _neo4j.AsyncSession();
        var studentIdList = new List<int>();
        var groupIdList = new List<int>();
        var parameters = new { LectureIds = LectureIds.Select(id => (long)id).ToList() };
        try
        {
            var result = await session.RunAsync(@"
                MATCH (l:Lecture)
                WHERE l.id IN $LectureIds
                OPTIONAL MATCH (s:Student)-[:ATTENDED]->(l)
                OPTIONAL MATCH (g:Group)-[:HAS_LECTURE]->(l)
                RETURN collect(DISTINCT s.id) AS StudentIds, collect(DISTINCT g.id) AS GroupIds", parameters);

            // 2. Получаем единственную запись из результата
            // collect() гарантирует, что будет одна строка, даже если списки пусты
            var record = await result.SingleAsync();

            // 3. Извлекаем списки ID из записи
            // Драйвер вернет списки как List<object>, нужно преобразовать к List<long>
            var groupIdObjects = record["GroupIds"].As<List<object>>();
            var studentIdObjects = record["StudentIds"].As<List<object>>();
            if (groupIdObjects != null)
            {
                groupIdList = groupIdObjects.Select(obj => Convert.ToInt32(obj)).ToList();
            }
            // Безопасное преобразование List<object> в List<long>
            if (studentIdObjects != null)
            {
                // Neo4j integer обычно -> long в C#
                studentIdList = studentIdObjects.Select(obj => Convert.ToInt32(obj)).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        return (studentIdList, groupIdList);
    }
}