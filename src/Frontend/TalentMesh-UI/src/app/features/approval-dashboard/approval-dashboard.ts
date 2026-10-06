import { DatePipe } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { AllocationService } from "../../core/services/allocation.service";
import { Allocation } from "../../models/allocation.model";
import { Project } from "../../models/project";
import { Employee } from "../../models/employee";
import { ProjectService } from "../../core/services/project.service";
import { EmployeeService } from "../../core/services/employee.service";

@Component({
    selector: 'app-approval-dashboard',
    standalone: true,
    imports: [DatePipe],
    styleUrl: './approval-dashboard.css',
    templateUrl: './approval-dashboard.html'
})
export class ApprovalDashboard implements OnInit {
    private readonly allocationService = inject(AllocationService);

    allocations = signal<Allocation[]>([]);
    isLoading = signal(false);
    error = signal('');
    selectedForRejection = signal<Allocation | null>(null);
    rejectionReason = signal('');
    isSubmittingRejection = signal(false);
    processingAllocationId = signal<string | null>(null);
    actionMessage = signal('');
    actionError = signal('');
    projects = signal<Project[]>([]);
    employees = signal<Employee[]>([]);
    projectService = inject(ProjectService);
    employeeService = inject(EmployeeService)

    loadReferenceData(): void {
        this.projectService.getProjects().subscribe({
            next: (projects) => this.projects.set(projects),
            error: (error) => {
                console.error('Failed to load projects:', error);
            }
        });

        this.employeeService.getAllEmployees().subscribe({
            next: (employees) => this.employees.set(employees),
            error: (error) => {
                console.error('Failed to load employees:', error);
            }
        });
    }

    ngOnInit(): void {
        this.loadPendingAllocations();
        this.loadReferenceData();
    }
    loadPendingAllocations(): void {
        this.isLoading.set(true);
        this.error.set('');

        this.allocationService.getAllocations('PendingApproval').subscribe({
            next: (allocations: Allocation[]) => {
                this.allocations.set(allocations);
                this.isLoading.set(false);
                this.error.set('');
            },
            error: (error: any) => {
                console.error('Failed to load allocation requests:', error);
                this.error.set(
                    error?.error?.message ??
                    error?.error?.title ??
                    'Unable to load pending allocation requests.'
                );
                this.isLoading.set(false);
            }
        })
    }
    getProjectName(projectId: string): string {
        return (
            this.projects().find(project => project.id === projectId)?.name
            ?? projectId
        );
    }

    getEmployeeName(employeeId: string): string {
        return (
            this.employees().find(employee => employee.id === employeeId)?.name
            ?? employeeId
        );
    }

    approveAllocation(allocation: Allocation): void {
        if (this.processingAllocationId()) return;

        this.processingAllocationId.set(allocation.id);
        this.actionMessage.set('');
        this.actionError.set('');

        this.allocationService.approveAllocation(allocation.id).subscribe({
            next: () => {
                this.processingAllocationId.set(null);
                this.actionMessage.set('Allocation request approved successfully.');
                this.loadPendingAllocations();
            },
            error: (error) => {
                console.error('Approval failed:', error);
                this.processingAllocationId.set(null);
                this.actionError.set(
                    error?.error?.message ??
                    error?.error?.title ??
                    'Unable to approve this allocation request.')
            }
        });
    }
    openRejectDialog(allocation: Allocation): void {
        if (this.processingAllocationId()) return;

        this.selectedForRejection.set(allocation);
        this.rejectionReason.set('');
        this.actionError.set('');
    }

    closeRejectDialog(): void {
        if (this.isSubmittingRejection()) return;

        this.selectedForRejection.set(null);
        this.rejectionReason.set('');
    }

    confirmRejection(): void {
        const allocation = this.selectedForRejection();
        const reason = this.rejectionReason().trim();

        if (!allocation || this.isSubmittingRejection()) return;

        if (!reason) {
            this.actionError.set('Please provide a rejection reason.');
            return;
        }

        this.isSubmittingRejection.set(true);
        this.processingAllocationId.set(allocation.id);
        this.actionError.set('');

        this.allocationService
            .rejectAllocation(allocation.id, reason)
            .subscribe({
                next: () => {
                    this.isSubmittingRejection.set(false);
                    this.processingAllocationId.set(null);
                    this.selectedForRejection.set(null);
                    this.rejectionReason.set('');
                    this.actionMessage.set('Allocation request rejected successfully.');
                    this.loadPendingAllocations();
                },
                error: (error) => {
                    console.error('Rejection failed:', error);
                    this.isSubmittingRejection.set(false);
                    this.processingAllocationId.set(null);
                    this.actionError.set(
                        error?.error?.message ??
                        error?.error?.title ??
                        'Unable to reject this allocation request.'
                    );
                }
            });
    }
}