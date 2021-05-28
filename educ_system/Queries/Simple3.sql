SELECT DISTINCT Courses.Name, Courses.Price
FROM Courses 
INNER JOIN Teachers ON Teachers.Id = Courses.TeacherId 
INNER JOIN Subjects ON Subjects.Id = Teachers.SubjectId
WHERE Subjects.Name = Õ;