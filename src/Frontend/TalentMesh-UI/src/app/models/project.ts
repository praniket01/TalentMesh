
export interface ProjectSkillRequirement {
    id : string;
    skillName : string;
    requiredLevel : string;
    requiredCount : number;
}

export interface Project {
    id : string;
    name : string;
    description : string;
    startDate : string;
    endDate:  string;
    clientName : string;
    status : string;
    skillRequirement : ProjectSkillRequirement[];
}