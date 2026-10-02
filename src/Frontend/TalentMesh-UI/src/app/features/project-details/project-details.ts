import { Component, inject, signal } from "@angular/core";
import { Project } from "../../models/project";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { ProjectService } from "../../core/services/project.service";
import { sign } from "crypto";
import { CandidateMatch } from "../../models/matching.model";
import { AllocationService } from "../../core/services/allocation.service";

@Component({
    selector: 'app-project-details',
    standalone: true,
    imports: [RouterLink],
    templateUrl: 'project-details.html',
    styleUrl: 'project-details.css'
})
export class ProjectDetails {
    project = signal<Project | null>(null);
    isLoading = signal(true);
    error = signal('');
    candidates = signal<CandidateMatch[]>([]);
    isMatching = signal(false);
    matchingError = signal('');
    showCandidates = signal(false);
    allocationEmployee = signal<CandidateMatch | null>(null);
    showAllocationDialog = signal(false);
    allocationPercentage = signal(0);
    isSubmittingAllocation = signal(false);

    allocationError = signal('');

    allocationSuccess = signal('');
    private readonly allocationService = inject(AllocationService);

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private projectService: ProjectService
    ) { }

    ngOnInit(): void {
        const projectId = this.route.snapshot.paramMap.get('id');

        if (!projectId) {
            this.error.set('Project ID is missing.');
            this.isLoading.set(false);
            return;
        }

        this.loadProject(projectId);
    }

    private loadProject(id: string): void {
        this.isLoading.set(true);

        this.projectService.getProjectbyId(id).subscribe({
            next: (project) => {
                this.project.set({
                    ...project,
                    skillRequirements: project.skillRequirements ?? []
                });

                this.isLoading.set(false);
            },

            error: (error) => {
                console.error('Failed to load project:', error);

                this.error.set(
                    'Unable to load project details.'
                );

                this.isLoading.set(false);
            }
        });
    }

    findCandidates(): void {
        const currentProject = this.project();

        if (!currentProject) return;


        this.isMatching.set(true);
        this.matchingError.set('');
        this.showCandidates.set(false);

        this.projectService.getMatchingCandidates(currentProject.id)
            .subscribe({
                next: (result) => {
                    this.candidates.set(result.candidates);
                    this.showCandidates.set(true);
                    this.isMatching.set(false);
                },
                error: (error) => {
                    console.error(
                        'Failed to find matching candidates:',
                        error
                    );

                    this.matchingError.set(
                        'Unable to find matching resources.'
                    );

                    this.isMatching.set(false);
                }
            })

    }

    requestAllocation(candidate: CandidateMatch): void {

        this.allocationEmployee.set(candidate);

        this.allocationPercentage.set(0);

        this.allocationError.set('');

        this.allocationSuccess.set('');

        this.showAllocationDialog.set(true);
    }
    goBack(): void {
        this.router.navigate(['/projects']);
    }

    closeAllocationDialog(): void {

        if (this.isSubmittingAllocation()) {
            return;
        }

        this.showAllocationDialog.set(false);

        this.allocationEmployee.set(null);
    }

    submitAllocation(): void {
        const project = this.project();
        const employee = this.allocationEmployee();

        if (!project || !employee) {
            return;
        }

        const percentage = this.allocationPercentage();

        if (percentage <= 0) {
            this.allocationError.set(
                'Allocation must be greater than 0%.'
            );

            return;
        }

        if (percentage > employee.availabilityPercentage) {
            this.allocationError.set(`Maximum available allocation is ` + `${employee.availabilityPercentage}%.`
            );

            return;
        }

        this.isSubmittingAllocation.set(true);
        this.allocationError.set('');


        this.allocationService.createAllocation({ projectId: project.id, employeeId: employee.employeeId, allocationPercentage: percentage })
            .subscribe({
                next: () => {
                    this.isSubmittingAllocation.set(false);
                    this.allocationSuccess.set(`Allocation request for ${percentage}% ` + `has been submitted successfully.`);

                    this.showAllocationDialog.set(false);
                },
                error: (error) => {

                    console.error(
                        'Allocation request failed:',
                        error
                    );

                    this.isSubmittingAllocation.set(false);

                    this.allocationError.set(
                        error?.error?.message ??
                        'Unable to submit allocation request.'
                    );
                }
            });
    }


}