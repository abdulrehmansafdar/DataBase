CREATE TABLE RubricTable (
    ID INT IDENTITY(1,1) NOT NULL,
    RubricID VARCHAR(50) NOT NULL,
    Details VARCHAR(50) NOT NULL,
    MeasurementLevel VARCHAR(50) NOT NULL,
    CONSTRAINT PK_RubricTable PRIMARY KEY (ID)
);