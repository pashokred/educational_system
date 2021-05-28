SELECT DISTINCT Teachers.Name
FROM Teachers
INNER JOIN Courses ON Teachers.Id = Courses.TeacherId
WHERE Courses.Price != Õ;