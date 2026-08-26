// DELIBERATE BREAK (M-08-1.4): a domain type referencing the Dataverse storage
// client. NEXUS_ARCHITECTURE_V2.md section 2.3 forbids this - the domain must
// not know how it is stored. Expect BoundaryTests.Domain_MustNotReference_
// Dataverse to go red. Reverted in the next commit.
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Nexus.Products.Chat.Domain.Common;

public static class DeliberateBoundaryViolation
{
    // Violation: the storage client has no business in the domain layer.
    public static ServiceClient? Client;
}
