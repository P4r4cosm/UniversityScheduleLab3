using University_Schedule_Lab1.Repositories;

namespace University_Schedule_Lab1.Services;

public class FindBadStudentsService
{
    private readonly ElasticMaterialsRepository _elasticRepository;
    private readonly LectureRepository _lectureRepository;
    private readonly ScheduleRepository _scheduleRepository;
    
    public FindBadStudentsService(ElasticMaterialsRepository repository, 
        LectureRepository lectureRepository,
        ScheduleRepository _scheduleRepository)
    {
        _elasticRepository = repository;
        _lectureRepository = lectureRepository;
        _scheduleRepository = _scheduleRepository;
    }

    public async Task GetStudents(string text)
    {
        
        //получаем лекции из elastic-а
        var result = await _elasticRepository.GetMaterialElasticByTextAsync(text);
        
        // берём только id
        var lecturesIds = result;
        
        //получаем id групп прикриплённых к этим лекциям
        var res = await _lectureRepository.GetGroupIdsByLectureId(lecturesIds);
        var GroupIds = res.Item2;

        //на основании id group и id лекции находим в postgres расписания лекций для нужных групп
        var schedules = _scheduleRepository.GetSchedulesByLectureAndGroupIds(lecturesIds, GroupIds);
        
    }
}