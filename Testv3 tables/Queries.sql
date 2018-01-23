use Testv3

SELECT COUNT(*) AS "TOTAL Number of Questions Related to PHYSICAL"
FROM Answers
INNER JOIN Questions 
ON Answers.QuestionID=Questions.QuestionID
WHERE Answers.UserID = 'cf86c4c8-85f8-40b2-be8e-f15891d573d4'
AND Questions.QuestionTag = 'Physical'

SELECT COUNT(*) AS "Number of AGREE(Positive) in PHYSICAL"
FROM Answers
INNER JOIN Questions 
ON Answers.QuestionID=Questions.QuestionID
WHERE Answers.UserID = 'cf86c4c8-85f8-40b2-be8e-f15891d573d4'
AND Questions.QuestionTag = 'Physical'
AND Questions.IsQuestionPositive = 'true'
AND Answers.Answer = '1';

SELECT COUNT(*) AS "Number of AGREE(Negative) in PHYSICAL"
FROM Answers
INNER JOIN Questions 
ON Answers.QuestionID=Questions.QuestionID
WHERE Answers.UserID = 'cf86c4c8-85f8-40b2-be8e-f15891d573d4'
AND Questions.QuestionTag = 'Physical'
AND Questions.IsQuestionPositive = 'false'
AND Answers.Answer = '1';

SELECT COUNT(*) AS "Number of AGREE(Neutral) in PHYSICAL"
FROM Answers
INNER JOIN Questions 
ON Answers.QuestionID=Questions.QuestionID
WHERE Answers.UserID = 'cf86c4c8-85f8-40b2-be8e-f15891d573d4'
AND Questions.QuestionTag = 'Physical'
AND Answers.Answer = '2';

