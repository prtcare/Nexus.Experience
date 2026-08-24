# AGENTS.md — Nexus.Experience (Chat product)

**Repository**: C:\Personal\Nexus.Experience · github.com/prtcare/Nexus.Experience · solution Nexus.Experience.slnx
**Is**: The Chat product — .NET API at `/api/v1` plus a React client. Owns all product data and experience. The reference implementation every future product follows. See README.md for the full is/is-not.
**This repo has no `docs\` folder of its own.** All cross-cutting documentation lives in the sibling repository, `..\NexusAI\docs\`.

## Read before implementing (always)

1. This file.
2. `..\NexusAI\docs\DOCUMENTATION_INDEX.md`
3. `..\NexusAI\docs\CURRENT_STATE.md`
4. `README.md` (this repository) — is/is-not, local dev commands, persistence and frontend notes.
5. Whatever the active implementation prompt names as task-specific reading.

If `..\NexusAI` is not present as a sibling folder, stop and report.

## Authoritative rules for this repository

Repository instructions in this file override a coding model's default conventions. Coding/naming/security/testing/git rules live in and are owned by the standards indexed in `..\NexusAI\docs\DOCUMENTATION_INDEX.md`, including `TYPESCRIPT_REACT_STANDARDS.md` for the client. The full model-independent development process is `..\NexusAI\docs\AI_DEVELOPMENT_GOVERNANCE.md`.

## The one rule specific to this repository

This product decides nothing — ranking, agent choice, model choice and prompt assembly happen in Nexus.Int over HTTP, and this repository never holds a model-provider credential. `ChatContextBundleMapper` flattens this product's entities into the canonical `ContextItem` shape; changing it changes answer quality silently, with no exception and no failing status code — treat it as the highest-risk file here.

## Before changing anything

Inspect existing implementation and naming before adding anything new. Confirm `git status` is clean and `git fsck` reports no corruption before starting — a `.git-broken\` folder still sits here pending `M-08-2.1`; do not delete it without architect approval. This repository is mid-migration from Dataverse to Azure SQL by the strangler pattern (ADR-014) — check which persistence implementation the area you're touching actually resolves to before assuming.

## What you may decide yourself / what requires architect approval / before declaring completion

Same boundary as `..\NexusAI\docs\AI_DEVELOPMENT_GOVERNANCE.md` defines. When in doubt, stop and report rather than guess.

## Known temporary mechanisms in this repository

See `..\NexusAI\docs\CURRENT_STATE.md`. As of 2026-08-23, verified directly: `ChatTurnIdentity` (`src\Nexus.Products.Chat.Application\Chat\Identity\ChatTurnIdentity.cs`) returns a hardcoded tenant (`nexus-dev`) and fixed permissions (`chat:send-message`) — no auth on either API. Its own code comment: "TODO(V2): replace with the real actor once Platform identity lands (decision D-1)."
