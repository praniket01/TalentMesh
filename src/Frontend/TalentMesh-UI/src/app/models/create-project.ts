
export interface ProjectskillRequirementsRequest {
    skillName: string;
    requiredLevel: string;
    requiredCount: number;
}

export interface CreateProjectRequest {
    name: string;
    description: string;
    clientName: string;
    startDate: string;
    endDate: string;
    skillRequirementss: ProjectskillRequirementsRequest[];
}