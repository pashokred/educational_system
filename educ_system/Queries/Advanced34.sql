SELECT S.Id
FROM Students S
WHERE S.Name = Y
AND NOT EXISTS
	((SELECT Courses.Id
      FROM Courses
	  INNER JOIN Groups ON Courses.Id = Groups.CourseId
	  INNER JOIN StudentGroups ON Groups.Id = StudentGroups.GroupId 
	  INNER JOIN Students ON Students.Id = StudentGroups.StudentId
      WHERE Students.Id = S.Id)
	 EXCEPT
	 (SELECT Courses.Id
	  FROM Courses))
AND NOT EXISTS
	((SELECT Courses.Id
	  FROM Courses)
	 EXCEPT
	 (SELECT Courses.Id
      FROM Courses
	  INNER JOIN Groups ON Courses.Id = Groups.CourseId
	  INNER JOIN StudentGroups ON Groups.Id = StudentGroups.GroupId 
	  INNER JOIN Students ON Students.Id = StudentGroups.StudentId
      WHERE Students.Id = S.Id));