SELECT T.Name
FROM Teachers AS T 
INNER JOIN Courses AS C ON C.TeacherId = T.Id
INNER JOIN Groups AS G ON G.CourseId = C.Id
INNER JOIN StudentGroups AS SG ON SG.GroupId = GroupId
INNER JOIN Students AS S ON S.Id = SG.StudentId
GROUP BY T.Name
HAVING COUNT(*) > X