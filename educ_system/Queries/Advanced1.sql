SELECT DISTINCT S.Name
FROM Subjects AS S 
INNER JOIN Teachers ON S.Id = Teachers.SubjectId
WHERE Teachers.Id IN (
	SELECT T.Id
	FROM Teachers AS T
	WHERE NOT EXISTS 
		((SELECT Courses.Price
		FROM Courses
		WHERE Courses.TeacherId = T.Id)
		EXCEPT 
		(SELECT Courses.Price
		FROM Courses
		WHERE Courses.TeacherId = T.Id AND Courses.TeacherId != Y
		))
)