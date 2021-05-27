SELECT Subjects.Name
FROM Subjects
WHERE NOT EXISTS (
     SELECT *
     FROM Teachers
     WHERE Teachers.SubjectId != Subjects.Id
)