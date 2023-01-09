import { Product } from "./product";

export class Order {
    constructor(
        public id?: string,
        public date?: Date,
        public username?: string,
        public product?: Product,
        public quantity?: number,
        public total?: number
    ) {}
}
