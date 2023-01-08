export class Error {
    constructor(
        public status?: number,
        public title?: string,
        public detail?: string
    ) {}
}
