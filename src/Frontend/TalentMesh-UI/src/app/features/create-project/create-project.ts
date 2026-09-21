import { Component, inject } from '@angular/core';
import {
    AbstractControl,
    FormArray,
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    ValidationErrors,
    ValidatorFn,
    Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { ProjectService } from '../../core/services/project.service';

type SkillForm = FormGroup<{
    skillName: ReturnType<FormBuilder['control']>;
    requiredLevel: ReturnType<FormBuilder['control']>;
    requiredCount: ReturnType<FormBuilder['control']>;
}>;

interface skillRequirementsForm {
    skillName: string;
    requiredLevel: string;
    requiredCount: number;
}

@Component({
    selector: 'app-create-project',
    imports: [ReactiveFormsModule],
    templateUrl: './create-project.html',
    styleUrl: './create-project.css'
})
export class CreateProject {

    private fb = inject(FormBuilder);
    private projectService = inject(ProjectService);
    private router = inject(Router);

    submitting = false;
    error = '';

    projectForm = this.fb.group({

        name: [
            '',
            [
                Validators.required,
                Validators.maxLength(200)
            ]
        ],

        description: [
            '',
            Validators.required
        ],

        clientName: [
            '',
            Validators.required
        ],

        startDate: [
            '',
            Validators.required
        ],

        endDate: [
            '',
            Validators.required
        ],

        skillRequirementss: this.fb.array([

        ])

    },
        {
            validators: this.dateRangeValidator()
        }
    );

    get skillRequirementss(): FormArray<FormGroup> {
        return this.projectForm.get(
            'skillRequirementss'
        ) as FormArray<FormGroup>;
    }

    addSkill(): void {

        const skill = this.fb.group({

            skillName: [
                '',
                Validators.required
            ],

            requiredLevel: [
                'Intermediate',
                Validators.required
            ],

            requiredCount: [
                1,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ]

        });

        this.skillRequirementss.push(skill);
    }

    removeSkill(index: number): void {
        this.skillRequirementss.removeAt(index);
    }
    dateRangeValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {

            const startDate = control.get('startDate')?.value;
            const endDate = control.get('endDate')?.value;

            if (!startDate || !endDate) {
                return null;
            }

            if (new Date(endDate) <= new Date(startDate)) {
                return {
                    invalidDateRange: true
                };
            }

            return null;
        };
    }
    submit(): void {

        if (this.projectForm.invalid) {

            this.projectForm.markAllAsTouched();

            return;
        }


        this.submitting = true;
        this.error = '';

        const formValue =
            this.projectForm.getRawValue();
        const skills =
            formValue.skillRequirementss as skillRequirementsForm[];

        const request = {
            name: formValue.name!,
            description: formValue.description!,
            clientName: formValue.clientName!,
            startDate: `${formValue.startDate}T00:00:00Z`,
            endDate: `${formValue.endDate}T00:00:00Z`,

            skillRequirementss:
                skills.map(skill => ({
                    skillName: skill.skillName!,
                    requiredLevel: skill.requiredLevel!,
                    requiredCount: skill.requiredCount!
                }))
        };

        this.projectService
            .createProject(request)
            .subscribe({

                next: () => {

                    this.submitting = false;

                    this.router.navigate(['/projects']);

                },

                error: (err) => {

                    console.error(err);

                    this.error =
                        'Unable to create project.';

                    this.submitting = false;
                }

            });
    }

    cancel(): void {
        this.router.navigate(['/projects']);
    }
}