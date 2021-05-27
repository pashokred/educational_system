SELECT Teachers.Name
FROM Teachers 
INNER JOIN Courses ON Teachers.Id = Courses.TeacherId
INNER JOIN Groups ON Groups.CourseId = Courses.Id
INNER JOIN StudentGroups ON StudentGroups.GroupId = Groups.Id
INNER JOIN Students ON Students.Id = StudentGroups.StudentId
WHERE Students.Name = X;