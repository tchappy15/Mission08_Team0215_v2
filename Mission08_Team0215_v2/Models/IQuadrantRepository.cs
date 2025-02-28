namespace Mission08_Team0215_v2.Models
{
    public interface IQuadrantRepository
    {
        IQueryable<Quadrant> Quadrants { get; }
        List<Category> Categories { get; } // Retrieves all categories


        Quadrant GetQuadrantById(int id);
        List<Category> GetCategories();
        public void AddQuadrant(Quadrant quadrant);
        public void UpdateQuadrant(Quadrant quadrant);
        void DeleteQuadrant(Quadrant recordToDelete); // Deletes a task

    }
}
