SELECT Students.Name 
FROM Students
WHERE Students.Id NOT IN (
	SELECT S.Id
	FROM Students AS S 
	INNER JOIN StudentGroups AS SG ON S.Id = SG.StudentId 
	INNER JOIN Groups AS G ON SG.GroupId = G.Id 
	INNER JOIN Courses AS C ON C.Id = G.CourseId
	INNER JOIN Teachers AS T ON T.Id = C.TeacherId
	INNER JOIN Subjects AS SB ON SB.Id = T.SubjectId
	WHERE SB.Name = X)