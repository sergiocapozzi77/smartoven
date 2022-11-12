#ifndef OVENSTATUS_H
#define OVENSTATUS_H

struct OvenStatus
{
    volatile int status; // 0:off 1:on
};

#endif