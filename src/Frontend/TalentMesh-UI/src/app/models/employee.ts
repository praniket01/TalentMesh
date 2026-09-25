
export interface Skill {
  id: string;
  name: string;
  category: string;
}

export interface EmployeeSkill {
  id: string;
  level: string;
  score: number;
  yearsOfExperience: number;
  skill: Skill;
}

export interface Employee{
    id : string;
    name : string;
    designation : string;
    experienceYears : string;
    location : string;
    allocationPercentage:  number;
    skills : EmployeeSkill[]
}