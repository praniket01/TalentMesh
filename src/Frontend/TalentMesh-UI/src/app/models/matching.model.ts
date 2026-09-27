

export interface CandidateMatch {
    employeeId : string;
    name : string; 
    designation : string;
    location : string;
    experienceYears : number;
    allocationPercentage : number;
    availabilityPercentage : number;
    skillMatch : number;
    availabilityScore : number;
    experienceScore : number;
    matchScore : number;
}

export interface MatchingResult {
    projectId : string;
    projectName : string;
    candidates : CandidateMatch[]
}