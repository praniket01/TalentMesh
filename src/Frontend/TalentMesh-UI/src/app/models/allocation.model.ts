export interface CreateAllocationRequest {
    projectId: string;
    employeeId: string;
    allocationPercentage: number;
}

export interface Allocation {
    id: string;
    projectId: string;
    employeeId: string;
    allocationPercentage: number;
    status: 'PendingApproval' | 'Approved' | 'Rejected';
    requestedBy: string;
    approvedBy?: string | null;
    requestedAt: string;
    approvedAt?: string | null;
    rejectionReason?: string | null;
}