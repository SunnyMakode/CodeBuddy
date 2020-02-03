import { Pagination } from './pagination';

export class PaginatedResult<T> {
    result: T; // This will be the result we received from the server from api
    pagination: Pagination; // This will be the paginated properties we received from the server
}
