SELECT DISTINCT Students.Name
FROM Students 
		INNER JOIN StudentGroups ON Students.Id = StudentGroups.StudentId 
		INNER JOIN Groups ON StudentGroups.GroupId = Groups.Id 
		INNER JOIN Courses ON Courses.Id = Groups.CourseId 
		INNER JOIN Types ON Courses.TypeId = Types.Id
WHERE Types.Name = X;