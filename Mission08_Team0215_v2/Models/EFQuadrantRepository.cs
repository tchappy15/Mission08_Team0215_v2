namespace Mission08_Team0215_v2.Models
{
    public class EFQuadrantRepository : IQuadrantRepository
    {
        private QuadrantsContext _context;
        public EFQuadrantRepository(QuadrantsContext temp)
        {
            _context = temp;
        }

        public IQueryable<Quadrant> Quadrants => _context.Quadrants;
        public List<Category> Categories => _context.Categories.ToList();

        public Quadrant GetQuadrantById(int taskId)
        {
            return _context.Quadrants.FirstOrDefault(q => q.TaskId == taskId);
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
        public void AddQuadrant(Quadrant quadrant)
        {
            _context.Add(quadrant);
            _context.SaveChanges();

        }
        public void UpdateQuadrant(Quadrant quadrant)
        {
            _context.Update(quadrant);
            _context.SaveChanges();
        }
        public void DeleteQuadrant(Quadrant quadrant)
        {

                _context.Quadrants.Remove(quadrant);
                _context.SaveChanges();
            
        }
    }
}
